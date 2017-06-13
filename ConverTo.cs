using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Practice
{
    class ConvertTo
    {

        public double Fahrenheit(double tempInCelcius)
        {
            return (Celcius(tempInCelcius)) * 9 / 5 + 32; ;
        }
        public double Celcius(double tempInKelvin)
        {
            return tempInKelvin - 273.15;
        }
    }
}
