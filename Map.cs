using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection_1._8._5
{
    class Map
    {
        private int _room = 1;
        private int _stage = 5;

        public int room
        {
            get
            {
                return _room;
            }
            set
            {
                _room = value;
            }
        }
        public int stage
        {
            get
            {
                return _stage;
            }
            set
            {
                _stage = value;
            }
        }
    }
}


