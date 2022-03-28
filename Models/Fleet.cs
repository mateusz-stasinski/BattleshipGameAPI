using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Fleet
    {
    }

    internal class CarrierSheep
    {
        const int size = 5;

        int[,] carrierSheep;

        CarrierSheep()
        {
            carrierSheep = new int[size, 3]; //{row position, column position, status}
        }
    }
}
