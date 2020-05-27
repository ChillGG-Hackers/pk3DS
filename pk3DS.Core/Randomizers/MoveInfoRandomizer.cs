using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pk3DS.Core.Structures;
using pk3DS.Core.Structures.PersonalInfo;


namespace pk3DS.Core.Randomizers
{
    public class MoveInfoRandomizer
    {
        private readonly GameConfig Config;
        private readonly int MaxMoveID;
        private readonly Move[] MoveData;
        private readonly PersonalInfo[] SpeciesStat;

        private readonly GenericRandomizer RandMove;

        public MoveInfoRandomizer(GameConfig config)
        {
            Config = config;
            MaxMoveID = config.Info.MaxMoveID;
            MoveData = config.Moves;
            SpeciesStat = config.Personal.Table;
            RandMove = new GenericRandomizer(Enumerable.Range(1, MaxMoveID - 1).ToArray());

            foreach (Move move in MoveData) {

            }
        }
    }
}
