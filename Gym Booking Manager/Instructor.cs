﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Booking_Manager
{
    internal class Instructor: IReservable, ICSVable, IComparable<Instructor>
    {
        private Category category;
        private String name;
        private readonly Calendar calendar;
        private int quantity = 1;

        public string Name { get => name; }

        public Instructor(Category category, String name)
        {
            this.name = name;
            this.category = category;
            this.calendar = new Calendar();
        }
        // Every class T to be used for DbSet<T> needs a constructor with this parameter signature. Make sure the object is properly initialized.
        public Instructor(Dictionary<String, String> constructionArgs)
        {
            this.name = constructionArgs[nameof(name)];
            if (!Category.TryParse(constructionArgs[nameof(category)], out this.category))
            {
                throw new ArgumentException("Couldn't parse a valid Space.Category value.", nameof(category));
            }

            this.calendar = new Calendar();
        }
        public int CompareTo(Instructor? other)
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
            return this.CSVify(); // TODO: Don't use CSVify. Make it more readable.
        }

        public string CSVify()
        {
            return $"{nameof(category)}:{category.ToString()},{nameof(name)}:{name}";
        }

        public enum Category
        {
            Coach,
            PT
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
        public void MakeReservation(Reservation.Category category, IReservingEntity owner, DateTime start, DateTime end)
        {
            // Check if the reservation is valid
            if (start > end)
            {
                throw new ArgumentException("Start time must be before end time.");
            }
            // Check if the reservation is available
            if (this.calendar.isAvailable(start, end))
            {
                // Make the reservation
                this.calendar.AddReservation(new Reservation(category, this.quantity, owner, start, end));
            }
            else
            {
                throw new ArgumentException("The reservation is not available.");
            }
        }

        public void CancelReservation()
        {

        }
        //added to be able to print instructor name
        //public override string ToString()
        //{
        //    return $"{name}";
        //}
    }
}
