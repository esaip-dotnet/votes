# Vote By Kinect Project

Kinect for Windows 

## What is this?

This repository contains samples submitted by the Kinect for our Vote project in Esaip.
The Goal of this Kinect project is to vote "Yes" or "No" with just one hand. After the vote, the result is directly send at the winphone listener.

## Architecture

The project is developed with visual studio in C# and XAML.

## How we can vote?

Before voting, we need to enter voter's name. Then, voter need to face the kinect in order to vote.
The right hand is to vote "Yes", the left hand is to vote "No".
To validate this vote, voter need to block his left or right hand during 3 seconds above his shoulder. If he lowers his hand before 3 seconds, the vote won't be taken into account.



