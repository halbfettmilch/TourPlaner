using System;
using System.Collections.Generic;
using Models;

namespace TourPlaner_andreas_BL
{
    internal class TourFactoryImpl : ITourFactory
    {
        public IEnumerable<Tour> GetItems()
        {
            return new List<Tour>()
            {
                new Tour() {Name= "Tour1"},
                new Tour() {Name= "Tour2"},
                new Tour() {Name= "Tour3"},
                new Tour() {Name= "Tour4"},
                new Tour() {Name= "Tour5"},
            };
        }

        public IEnumerable<Tour> Search(string itemname, bool caseSensetive = false)
        {
            throw new NotImplementedException();
        }
    }
}
