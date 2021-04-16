using System.Collections.Generic;
using Models;

namespace TourPlaner_andreas_BL
{
    public interface ITourFactory
    {
        IEnumerable<Tour> GetItems();
        IEnumerable<Tour> Search(string itemname, bool caseSensetive = false);
    }
}