using SpiderControl.Core.Common;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Exceptions;

namespace SpiderControl.Core.Models;

public class Spider
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Orientation Orientation { get; private set; }

    public Spider(int x, int y, Orientation orientation)
    {
        X = x;
        Y = y;
        Orientation = orientation;
    }

    public Result<Unit> MoveForward()
    {
        var nextPosition = GetNextForwardPosition();

        if (!nextPosition.IsSuccess) 
            return Result<Unit>.Failure(nextPosition.Error);

        X = nextPosition.Value.X;
        Y = nextPosition.Value.Y;

        return Result<Unit>.Success(Unit.Value);
    }

    public Result<Unit> RotateLeft()
    {
        Orientation = GetLeftOrientation();
        return Result<Unit>.Success(Unit.Value);
    }

    public Result<Unit> RotateRight()
    {
        Orientation = GetRightOrientation();
        return Result<Unit>.Success(Unit.Value);
    }

    public Result<Spider> GetNextForwardPosition()
    {
        var (nextX, nextY) = Orientation switch
        {
            Orientation.Up => (X, Y + 1),
            Orientation.Right => (X + 1, Y),
            Orientation.Down => (X, Y - 1),
            Orientation.Left => (X - 1, Y),
            _ => (X, Y)
        };

        return Result<Spider>.Success(new Spider(nextX, nextY, Orientation));
    }

    private Orientation GetRightOrientation() => Orientation switch
    {
        Orientation.Up => Orientation.Right,
        Orientation.Right => Orientation.Down,
        Orientation.Down => Orientation.Left,
        Orientation.Left => Orientation.Up,
        _ => throw new ArgumentOutOfRangeException()
    };

    private Orientation GetLeftOrientation() => Orientation switch
    {
        Orientation.Up => Orientation.Left,
        Orientation.Left => Orientation.Down,
        Orientation.Down => Orientation.Right,
        Orientation.Right => Orientation.Up,
        _ => throw new ArgumentOutOfRangeException()
    };

    public override string ToString()
    {
        var orientation = this.Orientation switch
        {
            Orientation.Up => "Up",
            Orientation.Right => "Right",
            Orientation.Down => "Down",
            Orientation.Left => "Left",
            _ => throw new InvalidSpiderToStringException($"Invalid orientation: {this.Orientation}")
        };

        return $"{this.X} {this.Y} {orientation}";
    }
}
