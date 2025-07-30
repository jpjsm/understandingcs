using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDive_WorkingWithArrays
{
    public class Skydiver
    {
        private Skydive[] dives;
        private string diverId;
        private double weight;
        private int generatedDives = 0;

        public Skydiver(string id, double weightKg, int divesToGenerate)
        {
            if (weightKg < 0)
            {
                // Assuming standard diver weight of 80 kg
                weightKg = 80;
            }

            if(divesToGenerate < 1)
            {
                // assuming default of 10 dives to generate
                divesToGenerate = 10;
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                id = "Diver " + DateTime.Now.ToString("yyyy-MM-dd HHmmss");
            }

            dives = new Skydive[divesToGenerate];
            diverId = id;
            weight = weightKg;
        }

        public bool AddDive(double finaltime, double delta, double drag, double area)
        {
            if (generatedDives < dives.Length)
            {
                dives[generatedDives] = new Skydive(finaltime, delta, drag, area, weight);
                generatedDives++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Skydive GetDive(int index)
        {
            if(index < 0 || index >= generatedDives)
            {
                return null;
            }

            return dives[index];
        }
    }
}
