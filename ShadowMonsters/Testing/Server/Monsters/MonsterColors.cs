﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Server.Monsters
{
    public static class MonsterColors
    {
        public static List<Color> EarthTones = new List<Color>
        {
            {Color.FromArgb(126,113,93) },
            {Color.FromArgb(87,76,60) },
            {Color.FromArgb(109,93,71) },
            {Color.FromArgb(95,71,47) },
            {Color.FromArgb(96,74,50) },
            {Color.FromArgb(96,79,49) },
            {Color.FromArgb(100,85,48) },
            {Color.FromArgb(102,88,48) },
            {Color.FromArgb(104,91,47) },
            {Color.FromArgb(100,88,48) },
            {Color.FromArgb(97,85,49) },
            {Color.FromArgb(90,84,51) },
            {Color.FromArgb(86,82,52) },
            {Color.FromArgb(81,79,51) },
            {Color.FromArgb(77,76,51) },
            {Color.FromArgb(77,78,57) },
            {Color.FromArgb(78,79,62) },
            {Color.FromArgb(143,133,117) },
            {Color.FromArgb(89,80,67) },
            {Color.FromArgb(109,93,73) },
            {Color.FromArgb(105,70,41) },
            {Color.FromArgb(107,76,47) },
            {Color.FromArgb(107,86,45) },
            {Color.FromArgb(115,98,43) },
            {Color.FromArgb(119,104,43) },
            {Color.FromArgb(123,110,41) },
            {Color.FromArgb(115,104,43) },
            {Color.FromArgb(109,98,45) },
            {Color.FromArgb(95,96,49) },
            {Color.FromArgb(87,92,51) },
            {Color.FromArgb(77,86,49) },
            {Color.FromArgb(69,80,49) },
            {Color.FromArgb(69,84,61) },
            {Color.FromArgb(71,86,71) },
            {Color.FromArgb(160,153,141) },
            {Color.FromArgb(91,84,74) },
            {Color.FromArgb(109,93,75) },
            {Color.FromArgb(115,69,35) },
            {Color.FromArgb(118,78,44) },
            {Color.FromArgb(118,93,41) },
            {Color.FromArgb(130,111,38) },
            {Color.FromArgb(136,120,38) },
            {Color.FromArgb(142,129,35) },
            {Color.FromArgb(130,120,38) },
            {Color.FromArgb(121,111,41) },
            {Color.FromArgb(100,108,47) },
            {Color.FromArgb(88,102,50) },
            {Color.FromArgb(73,93,47) },
            {Color.FromArgb(61,84,47) },
            {Color.FromArgb(61,90,65) },
            {Color.FromArgb(64,93,80) },
            {Color.FromArgb(177,173,165) },
            {Color.FromArgb(93,88,81) },
            {Color.FromArgb(109,93,77) },
            {Color.FromArgb(125,68,29) },
            {Color.FromArgb(129,80,41) },
            {Color.FromArgb(129,100,37) },
            {Color.FromArgb(145,124,33) },
            {Color.FromArgb(153,136,33) },
            {Color.FromArgb(161,148,29) },
            {Color.FromArgb(145,136,33) },
            {Color.FromArgb(133,124,37) },
            {Color.FromArgb(105,120,45) },
            {Color.FromArgb(89,112,49) },
            {Color.FromArgb(69,100,45) },
            {Color.FromArgb(53,88,45) },
            {Color.FromArgb(53,96,69) },
            {Color.FromArgb(57,100,89) },
            {Color.FromArgb(194,193,189) },
            {Color.FromArgb(95,92,88) },
            {Color.FromArgb(109,93,79) },
            {Color.FromArgb(135,67,23) },
            {Color.FromArgb(140,82,38) },
            {Color.FromArgb(140,107,33) },
            {Color.FromArgb(160,137,28) },
            {Color.FromArgb(170,152,28) },
            {Color.FromArgb(180,167,23) },
            {Color.FromArgb(160,152,28) },
            {Color.FromArgb(145,137,33) },
            {Color.FromArgb(110,132,43) },
            {Color.FromArgb(90,122,48) },
            {Color.FromArgb(65,107,43) },
            {Color.FromArgb(45,92,43) },
            {Color.FromArgb(45,102,73) },
            {Color.FromArgb(50,107,98) },
            {Color.FromArgb(213,213,213) },
            {Color.FromArgb(98,98,98) },
            {Color.FromArgb(106,92,82) },
            {Color.FromArgb(148,62,15) },
            {Color.FromArgb(153,87,32) },
            {Color.FromArgb(153,118,27) },
            {Color.FromArgb(177,151,21) },
            {Color.FromArgb(189,169,18) },
            {Color.FromArgb(201,188,15) },
            {Color.FromArgb(180,170,21) },
            {Color.FromArgb(157,150,28) },
            {Color.FromArgb(118,144,40) },
            {Color.FromArgb(92,135,45) },
            {Color.FromArgb(57,118,40) },
            {Color.FromArgb(36,96,36) },
            {Color.FromArgb(32,110,79) },
            {Color.FromArgb(40,114,110) },
            {Color.FromArgb(214,213,209) },
            {Color.FromArgb(118,117,112) },
            {Color.FromArgb(124,112,99) },
            {Color.FromArgb(159,87,43) },
            {Color.FromArgb(164,108,57) },
            {Color.FromArgb(164,133,53) },
            {Color.FromArgb(184,161,48) },
            {Color.FromArgb(194,176,46) },
            {Color.FromArgb(204,192,43) },
            {Color.FromArgb(186,177,48) },
            {Color.FromArgb(167,160,54) },
            {Color.FromArgb(134,155,64) },
            {Color.FromArgb(113,148,68) },
            {Color.FromArgb(84,133,64) },
            {Color.FromArgb(66,115,61) },
            {Color.FromArgb(63,127,96) },
            {Color.FromArgb(69,130,122) },
            {Color.FromArgb(215,213,205) },
            {Color.FromArgb(138,136,126) },
            {Color.FromArgb(142,132,116) },
            {Color.FromArgb(170,112,71) },
            {Color.FromArgb(175,129,82) },
            {Color.FromArgb(175,148,79) },
            {Color.FromArgb(191,171,75) },
            {Color.FromArgb(199,183,74) },
            {Color.FromArgb(207,196,71) },
            {Color.FromArgb(192,184,75) },
            {Color.FromArgb(177,170,80) },
            {Color.FromArgb(150,166,88) },
            {Color.FromArgb(134,161,91) },
            {Color.FromArgb(111,140,88) },
            {Color.FromArgb(96,134,86) },
            {Color.FromArgb(94,144,113) },
            {Color.FromArgb(98,146,134) },
            {Color.FromArgb(216,213,201) },
            {Color.FromArgb(158,155,140) },
            {Color.FromArgb(160,152,133) },
            {Color.FromArgb(181,137,99) },
            {Color.FromArgb(186,150,107) },
            {Color.FromArgb(186,169,105) },
            {Color.FromArgb(198,181,102) },
            {Color.FromArgb(204,190,102) },
            {Color.FromArgb(210,200,99) },
            {Color.FromArgb(198,191,102) },
            {Color.FromArgb(187,180,106) },
            {Color.FromArgb(166,177,112) },
            {Color.FromArgb(155,174,114) },
            {Color.FromArgb(138,163,112) },
            {Color.FromArgb(126,153,111) },
            {Color.FromArgb(125,161,130) },
            {Color.FromArgb(127,162,146) },
            {Color.FromArgb(217,213,197) },
            {Color.FromArgb(178,174,154) },
            {Color.FromArgb(178,172,150) },
            {Color.FromArgb(192,162,127) },
            {Color.FromArgb(197,171,132) },
            {Color.FromArgb(197,178,131) },
            {Color.FromArgb(205,191,129) },
            {Color.FromArgb(209,197,130) },
            {Color.FromArgb(213,204,127) },
            {Color.FromArgb(204,198,129) },
            {Color.FromArgb(197,190,132) },
            {Color.FromArgb(182,188,136) },
            {Color.FromArgb(176,187,137) },
            {Color.FromArgb(165,178,136) },
            {Color.FromArgb(156,172,136) },
            {Color.FromArgb(156,178,147) },
            {Color.FromArgb(156,178,158) },
            {Color.FromArgb(218,213,193) },
            {Color.FromArgb(198,193,168) },
            {Color.FromArgb(196,192,167) },
            {Color.FromArgb(203,187,155) },
            {Color.FromArgb(208,192,157) },
            {Color.FromArgb(208,193,157) },
            {Color.FromArgb(212,201,156) },
            {Color.FromArgb(214,204,158) },
            {Color.FromArgb(216,208,155) },
            {Color.FromArgb(210,205,156) },
            {Color.FromArgb(207,200,158) },
            {Color.FromArgb(198,199,160) },
            {Color.FromArgb(197,200,160) },
            {Color.FromArgb(192,193,160) },
            {Color.FromArgb(186,191,161) },
            {Color.FromArgb(187,195,164) },
            {Color.FromArgb(185,194,170) },
        };

        public static List<Color> Metals = new List<Color>
        {
             Color.FromArgb(212, 175, 55), //gold
             Color.FromArgb(192,192,192),
             Color.FromArgb(184,115,51),
             Color.FromArgb(181,166,66),
             Color.FromArgb(230,232,250),
             Color.FromArgb(35,107,142),
             Color.FromArgb(140,120,83),
             Color.FromArgb(155,17,30),

        };

    }

    //this is how you can change the prefab's color
    //renderer.material.color = ; //gold
    //renderer.material.color = Color.FromArgb(0, 67, 200, 50);
}