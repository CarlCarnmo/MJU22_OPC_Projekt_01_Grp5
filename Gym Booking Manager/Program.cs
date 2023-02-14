﻿using Gym_Booking_Manager;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

#if DEBUG
[assembly: InternalsVisibleTo("Tests")]
#endif
namespace Gym_Booking_Manager
{
    internal class Program
    {

        static void Main(string[] args)
        {
            /*
                        Customer ruben = new Customer("Ruben");
                        Customer david = new Customer("David");
                        Customer adam = new Customer("Adam");
                        TimeSlot timeSlot = new TimeSlot(new DateTime(2023, 10, 10, 20, 30, 00), new DateTime(2023, 10, 10, 21, 00, 00));
                        Console.WriteLine(timeSlot);
                        Instructor instructor = new Instructor("Tim");
                        Space hall = new Space(0, "Hall");
                        Equipment treadmill = new Equipment(1);
                        GroupActivity activity1 = new GroupActivity("A1", 2, timeSlot, instructor, hall, treadmill);
                        activity1.SignUp(ruben);
                        activity1.SignUp(david);
                        activity1.SignUp(adam);
                        foreach(Customer c in activity1.Participants)
                        {
                            Console.WriteLine(c.name);
                        }
            */
            GymDatabaseContext userDB = new GymDatabaseContext();
            //Customer ruben = new Customer("Ruben", "011-131313", "ruben@test.se", Guid.NewGuid());
            //Customer david = new Customer("David", "011-131313", "asd", Guid.NewGuid());
            //userDB.Create<Customer>(ruben);
            //userDB.Create<Customer>(david);
            //userDB.Create<Customer>(ruben);
            GymDatabaseContext spaceDB = new GymDatabaseContext();
            foreach (Space s in spaceDB.Read<Space>())
            {
                Console.WriteLine(s);
            }
            foreach (Customer c in userDB.Read<Customer>("email", "jsakda@se.se"))
            {
                Console.WriteLine(c);
            }
            DateTime d1 = new DateTime(year: 2023, month: 10, day: 10, hour: 12, minute: 00, second:00);
            DateTime d2 = new DateTime(year: 2023, month: 10, day: 11);
            TimeSlot timeSlot = new TimeSlot(d1);
            TimeSlot timeSlot2 = new TimeSlot(d2);
            TimeSlot timeSlot3 = new TimeSlot(DateTime.Now);
            Console.WriteLine(timeSlot);
            Console.WriteLine(timeSlot2);
            Console.WriteLine(timeSlot3);
            foreach (Staff s in userDB.Read<Staff>())
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter your phone number: ");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter your email: ");
            string email = Console.ReadLine();
            Guid id = Guid.NewGuid();
            Console.WriteLine("Enter your choice (0 for Customer, 1 for Staff, 2 for Admin): ");
            int choice = int.Parse(Console.ReadLine());
            


            User user = User.ChooseUserType(name, phone, email, id, choice);
            if(user.GetPerm() == 0)
            {
               userDB.Create<Customer>(user as Customer);
            }
            else if(user.GetPerm() == 1)
            {
                userDB.Create<Staff>(user as Staff);
            }
            else if (user.GetPerm() == 2)
            {
                userDB.Create<Admin>(user as Admin);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
            

            int option = 0;
            do
            {
                Console.WriteLine("Option 1");
                Console.WriteLine("Option 2");
                Console.WriteLine("Option 3");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        break;
                    case 2:
                        Console.WriteLine("Option 2");
                        break;
                    case 3:
                        Console.WriteLine("Option 3");
                        break;
                    case 4:
                        Console.WriteLine("Exiting the menu...");
                        break;
                    default:
                        Console.WriteLine("Invalid option, no workie.");
                        break;
                }
                Console.WriteLine();
            } while (option != 4);
            
        }
        // Static methods for the program
    }
}