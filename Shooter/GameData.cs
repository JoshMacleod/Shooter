using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    public class GameData
    {
        private int _hits = 0;

        public int hits
        {
            get { return _hits; }
            set { _hits = value; }
        }

        private int _misses = 0;

        public int misses
        {
            get { return _misses; }
            set { _misses = value; }
        }

        private int _totalShots = 0;

        public int totalShots
        {
            get { return _totalShots; }
            set { _totalShots = value; }
        }

        private double _avgHits = 0;

        public double avgHits
        {
            get { return _avgHits; }
            set { _avgHits = value; }
        }
    }
}
