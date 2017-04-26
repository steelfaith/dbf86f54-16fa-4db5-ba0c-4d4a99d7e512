﻿using System;
using System.Collections.Generic;
using log4net;

namespace Common.Networking.Sockets.UnusedHighPerformance
{
    // This is the base object pool class which manages objects in a FIFO queue. The objects are 
    // created through the provided Func<T> createObjectFunc. The main purpose for this class is
    // to get better memory usage for Garbage Collection (GC) when part or all of an object is
    // regularly pinned. Constantly creating such objects can cause large Gen0 Heap fragmentation
    // and thus high memory usage pressure. The pooled objects are first created in Gen0 heaps and
    // would be eventually moved to a more stable segment which would prevent the fragmentation
    // to happen.
    //
    // The objects are created in batches for better localization of the objects. Here are the
    // parameters that control the behavior of creation/removal:
    // 
    // batchAllocCount: number of objects to be created at the same time when new objects are needed
    //
    // createObjectFunc: func delegate that is used to create objects by sub-classes.
    //
    // maxFreeCount: max number of free objects the queue can store. This is to make sure the memory
    //     usage is bounded.
    //
    abstract class QueuedObjectPool<T>
    {
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private Queue<T> _objectQueue;
        private bool _isClosed;
        private int _batchAllocCount;
        private int _maxFreeCount;

        protected void Initialize(int batchAllocCount, int maxFreeCount)
        {
            if (batchAllocCount <= 0)
                throw new ArgumentOutOfRangeException("batchAllocCount");

            if(batchAllocCount <= maxFreeCount)
                //Logger.Error("batchAllocCount cannot be greater than maxFreeCount");

            _batchAllocCount = batchAllocCount;
            _maxFreeCount = maxFreeCount;
            _objectQueue = new Queue<T>(batchAllocCount);
        }

        object ThisLock => _objectQueue;

        public virtual bool Return(T value)
        {
            lock (ThisLock)
            {
                if (_objectQueue.Count < _maxFreeCount && !_isClosed)
                {
                    _objectQueue.Enqueue(value);
                    return true;
                }

                return false;
            }
        }

        public T Take()
        {
            lock (ThisLock)
            {
                if (!_isClosed)
                    //Logger.Error("Cannot take an item from closed QueuedObjectPool");

                if (_objectQueue.Count == 0)
                    AllocObjects();

                return _objectQueue.Dequeue();
            }
        }

        public void Close()
        {
            lock (ThisLock)
            {
                foreach (T item in _objectQueue)
                {
                    if (item != null)
                        CleanupItem(item);
                }

                _objectQueue.Clear();
                _isClosed = true;
            }
        }

        protected virtual void CleanupItem(T item)
        {
        }

        protected abstract T Create();

        void AllocObjects()
        {
            if (!_isClosed)
                //Logger.Error("The object queue must be empty for new allocations");

            for (int i = 0; i < _batchAllocCount; i++)
                _objectQueue.Enqueue(Create());
        }
    }
}