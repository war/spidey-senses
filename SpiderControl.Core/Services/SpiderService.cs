﻿using SpiderControl.Core.Models;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Core.Services;

public class SpiderService : ISpiderService
{
    public SpiderService()
    {
        
    }

    public SpiderModel CreateSpider(int x, int y, Orientation orientation)
    {
        return new SpiderModel(x, y, orientation);
    }

    public void MoveForward(SpiderModel model)
    {
    }

    public void TurnLeft(SpiderModel model)
    {
    }

    public void TurnRight(SpiderModel model)
    {
    }
}
