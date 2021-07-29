using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WrapModeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void WrapModeSimplePasses()
    {
        // Use the Assert class to test conditions
    }


    [TestCase(WrapMode.Loop,0,2, ExpectedResult = 1)]
    [TestCase(WrapMode.Loop,1,2, ExpectedResult = 2)]
    [TestCase(WrapMode.Loop,2,2, ExpectedResult = 0)]

    [TestCase(WrapMode.PingPong,-1,2, ExpectedResult = 0)]
    [TestCase(WrapMode.PingPong,0,2, ExpectedResult = 1)]
    [TestCase(WrapMode.PingPong,1,2, ExpectedResult = 2)]
    [TestCase(WrapMode.PingPong,2,2, ExpectedResult = 1)]
    [TestCase(WrapMode.PingPong,3,2, ExpectedResult = 0)]
    public int NextIndex(WrapMode wrap, int currentIndex, int length)
    {
        switch (wrap)
        {
            case WrapMode.Once:
                currentIndex++;
                return currentIndex == length ? 0 : currentIndex;
            case WrapMode.Loop:
                if (currentIndex >= length)
                    currentIndex = 0;
                else
                    currentIndex++;
                break;
            case WrapMode.PingPong:

                currentIndex= Mathf.RoundToInt(Mathf.PingPong(currentIndex+1, length));
                break;
            case WrapMode.Default:
            case WrapMode.ClampForever:
                if (currentIndex >= length-1)
                    return length - 1;
                ++currentIndex;
                break;
        }
        return currentIndex;
    }
}
