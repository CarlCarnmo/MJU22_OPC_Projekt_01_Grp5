﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Gym_Booking_Manager.Space;

namespace Gym_Booking_Manager
{
    internal class Equipment: IReservable, ICSVable, IComparable<Equipment>
    {
        //private enum Category with 2 subcategories large equipment and sports equipment where we list the diffrenet kinds of each equipment subcategory?
        private Category category;
        private String name;
        private readonly Calendar calendar;
        private int quantity;
        private bool largeEquipment;

        public string Name { get => name; }
        public int Quantity { get => quantity; }

        public Equipment(Category category, String name, int quantity, bool largeEquipment = true)
        {
            this.category = category;
            this.name = name;
            this.quantity = quantity;
            this.largeEquipment = largeEquipment;
            this.calendar = new Calendar();
        }
        // Every class T to be used for DbSet<T> needs a constructor with this parameter signature. Make sure the object is properly initialized.
        public Equipment(Dictionary<String, String> constructionArgs)
        {
            this.name = constructionArgs[nameof(name)];
            this.quantity = int.Parse(constructionArgs[nameof(quantity)]);
            this.largeEquipment = bool.Parse(constructionArgs[nameof(largeEquipment)]);
            if (!Category.TryParse(constructionArgs[nameof(category)], out this.category))
            {
                throw new ArgumentException("Couldn't parse a valid Space.Category value.", nameof(category));
            }

            this.calendar = new Calendar();
        }
        public int CompareTo(Equipment? other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            // Sort primarily on category.
            if (this.category != other.category) return this.category.CompareTo(other.category);
            // When category is the same, sort on name.
            return this.name.CompareTo(other.name);
        }
        public override string ToString()
        {
            if (!this.largeEquipment)
                return $"{this.name}, quantity:{this.quantity}, category:{this.category}, small equipment";
            else
                return $"{this.name}, quantity:{this.quantity}, category:{this.category}, large equipment";
        }

        // Every class C to be used for DbSet<C> should have the ICSVable interface and the following implementation.
        public string CSVify()
        {
            return $"{nameof(category)}:{category.ToString()},{nameof(name)}:{name},{nameof(quantity)}:{quantity.ToString()},{nameof(largeEquipment)}:{largeEquipment.ToString()}";
        }

        //This enum contains just exampels of equipment
        public enum Category
        {
            Machines, 
            Gym, 
            Bench,
            Dumbbells,
            Treadmill,
            Rackets
        }
        public void ViewTimeTable()
        {
            // Fetch
            List<Reservation> tableSlice = this.calendar.GetSlice();
            // Show?
            foreach (Reservation reservation in tableSlice)
            {
                Console.WriteLine(reservation);
            }

        }

        public void MakeReservation(Reservation.Category category, IReservingEntity owner, TimeSlot timeslot)
        {
            // Check if the reservation is valid
            if (timeslot.Start > timeslot.End)
            {
                throw new ArgumentException("Start time must be before end time.");
            }
            // Check if the reservation is available
            if (this.calendar.isAvailable(timeslot.Start, timeslot.End))
            {
                // Make the reservation
                if (this.quantity > 0)
                {
                    this.calendar.AddReservation(new Reservation(category, this.quantity, owner, timeslot));
                    this.quantity -= 1;
                }
                else
                {
                    Console.WriteLine("Not available! STUPID!");
                }
            }
            else
            {
                throw new ArgumentException("The reservation is not available.");
            }
        }

        public void CancelReservation()
        {
            //ToDO
        }
        // Static methods for the program
        public static void BookEquipment(GymDatabaseContext DB, User user)
        {
            int i = 1;
            foreach (Equipment equipment in DB.Read<Equipment>())
            {
                Console.WriteLine(i + ": " + equipment);
                i++;
            }
            Console.WriteLine("Choose equipment:");
            int choice = Convert.ToInt32(Console.ReadLine());
            Equipment equipment1 = DB.Read<Equipment>()[choice - 1];
            Console.WriteLine("Choose time:(yyyy-mm-dd hh:mm)");
            DateTime time = Convert.ToDateTime(Console.ReadLine());
            if (time < DateTime.Now)
            {
                Console.WriteLine("You can't book a time in the past.");
                return;
            }
            TimeSlot timeSlot = new TimeSlot(time);
            Console.WriteLine("Choose user:");
            int j = 1;
            Console.WriteLine("Select a category for your reservation:");
            int k = 1;
            foreach (string category in Enum.GetNames(typeof(Reservation.Category)))
            {
                Console.WriteLine(k + ": " + category);
                k++;
            }
            Reservation.Category choice3 = (Reservation.Category)Convert.ToInt32(Console.ReadLine()) - 1;
            equipment1.MakeReservation(choice3, user, timeSlot);
            equipment1.ViewTimeTable();
        }
    }
}
