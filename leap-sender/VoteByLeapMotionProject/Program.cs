
﻿/******************************************************************************\
* Copyright (C) 2012-2014 Leap Motion, Inc. All rights reserved.               *
* Leap Motion proprietary and confidential. Not for distribution.              *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement         *
* between Leap Motion and you, your company or other organization.             *
* @Authors : Corentin Chevallier                                               *
* @Promotion : IR2016                                                          *
* @Tutor : Jean-Philippe Gouigoux                                              *
* @Date 23/12/2015 (DD/MM/YY)                                                  *
* @Brief : Leap Motion vote application                                        *
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoteByLeapMotionProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LeapMotionVoteSystemWindow());
        }
    }
}
