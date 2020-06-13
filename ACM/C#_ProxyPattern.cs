using System;

interface ICar
{
    void DriveCar();
}

// Real Object
public class Car : ICar
{
    public void DriveCar()
    {
        Console.WriteLine("Car has been driven!");
    }
}

// Proxy Object
public class ProxyCar : ICar
{
    private Driver driver;
    private ICar realCar;

    public ProxyCar(Driver driver)
    {
        this.driver = driver;
        this.realCar = new Car();
    }

    public void DriveCar()
    {
        if (driver.Age < 16)
            Console.WriteLine("Sorry, the driver is too young to drive.");
        else
            this.realCar.DriveCar();
     }
}

public class Driver
{
    public int Age { get; set; }

    public Driver(int age)
    {
        this.Age = age;
    }
}


class MainClass {
  public static void Main (string[] args) {
   // Console.WriteLine ("Hello World");
    ICar car = new ProxyCar(new Driver(16));
    car.DriveCar();

    car = new ProxyCar(new Driver(15));
    car.DriveCar();
  }
}