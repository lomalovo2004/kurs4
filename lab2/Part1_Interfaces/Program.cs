using System;
using System.Collections.Generic;
using System.IO;

namespace Part1_Interfaces;

public record Point(int X, int Y);

public interface IMovable
{
    void Move(int dx, int dy);
}

public sealed class MovablePoint : IMovable
{
    public Point Position { get; private set; } = new(0, 0);

    public void Move(int dx, int dy)
    {
        Position = new Point(Position.X + dx, Position.Y + dy);
    }

    public override string ToString() => $"({Position.X}; {Position.Y})";
}

public interface IDrawable
{
    void Draw();
}

public sealed class Circle : IDrawable
{
    public double Radius { get; init; }

    public void Draw() => Console.WriteLine($"Круг с радиусом {Radius} нарисован.");
}

public sealed class RectangleShape : IDrawable
{
    public double Width { get; init; }
    public double Height { get; init; }

    public void Draw() => Console.WriteLine($"Прямоугольник {Width}x{Height} нарисован.");
}

public interface IShape
{
    double GetArea();
    void Draw();
}

public sealed class AreaCircle : IShape
{
    public double Radius { get; init; }

    public double GetArea() => Math.PI * Radius * Radius;

    public void Draw() => Console.WriteLine("Отрисовка круга.");
}

public sealed class AreaRectangle : IShape
{
    public double Width { get; init; }
    public double Height { get; init; }

    public double GetArea() => Width * Height;

    public void Draw() => Console.WriteLine("Отрисовка прямоугольника.");
}

public interface I3DShape : IShape
{
    double GetVolume();
}

public sealed class Cube : I3DShape
{
    public double Side { get; init; }

    public double GetArea() => 6 * Side * Side;
    public double GetVolume() => Side * Side * Side;
    public void Draw() => Console.WriteLine($"Куб со стороной {Side}.");
}

public interface IPrinter
{
    void Print(string document);
}

public interface IScanner
{
    string Scan();
}

public interface IFax
{
    void SendFax(string number, string document);
}

public sealed class Printer : IPrinter
{
    public void Print(string document) => Console.WriteLine($"Печать: {document}");
}

public sealed class Scanner : IScanner
{
    public string Scan() => "Отсканированный документ";
}

public sealed class MultifunctionDevice : IPrinter, IScanner, IFax
{
    public void Print(string document) => Console.WriteLine($"МФУ печатает: {document}");
    public string Scan() => "МФУ: отсканированный документ";
    public void SendFax(string number, string document) =>
        Console.WriteLine($"Факс отправлен на {number}: {document}");
}

public interface IPayable
{
    void Pay(decimal amount);
}

public sealed class CreditCard : IPayable
{
    public void Pay(decimal amount) => Console.WriteLine($"Оплата картой: {amount:0.00} руб.");
}

public sealed class Cash : IPayable
{
    public void Pay(decimal amount) => Console.WriteLine($"Оплата наличными: {amount:0.00} руб.");
}

public interface ILogger
{
    void Log(string message);
}

public sealed class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

public sealed class FileLogger : ILogger
{
    private readonly string _filePath;

    public FileLogger(string filePath) => _filePath = filePath;

    public void Log(string message) =>
        File.AppendAllText(_filePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
}

internal static class Program
{
    private static void ProcessPayment(IPayable payment, decimal amount) => payment.Pay(amount);

    private static void PrintShapeInfo(IShape shape) =>
        Console.WriteLine($"Площадь фигуры: {shape.GetArea():0.00}");

    private static void DoWork(ILogger logger) =>
        logger.Log("Выполнена операция приложения.");

    public static void Main()
    {
        Console.WriteLine("=== Лабораторная работа №2. Часть 1: Интерфейсы ===");

        var point = new MovablePoint();
        point.Move(3, 4);
        Console.WriteLine($"IMovable: точка после перемещения {point}");

        var drawable = new List<IDrawable>
        {
            new Circle { Radius = 5 },
            new RectangleShape { Width = 4, Height = 6 }
        };
        foreach (var item in drawable)
            item.Draw();

        PrintShapeInfo(new AreaCircle { Radius = 3 });
        PrintShapeInfo(new AreaRectangle { Width = 4, Height = 5 });

        var cube = new Cube { Side = 2 };
        cube.Draw();
        Console.WriteLine($"Площадь поверхности куба: {cube.GetArea():0.00}");
        Console.WriteLine($"Объем куба: {cube.GetVolume():0.00}");

        IPrinter printer = new Printer();
        printer.Print("Документ 1");

        IScanner scanner = new Scanner();
        Console.WriteLine(scanner.Scan());

        var mfd = new MultifunctionDevice();
        mfd.Print("Документ 2");
        Console.WriteLine(mfd.Scan());
        mfd.SendFax("+7-900-000-00-00", "Документ 3");

        ProcessPayment(new CreditCard(), 1500);
        ProcessPayment(new Cash(), 500);

        ILogger consoleLogger = new ConsoleLogger();
        DoWork(consoleLogger);

        string logFile = Path.Combine(AppContext.BaseDirectory, "application.log");
        ILogger fileLogger = new FileLogger(logFile);
        DoWork(fileLogger);

        Console.WriteLine("=== Часть 1 завершена ===");
    }
}
