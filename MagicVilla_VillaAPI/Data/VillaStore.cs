﻿using MagicVilla_VillaAPI.Model.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        //Here we will store the data for vill
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO { Id = 1, Name = "Pool View", Sqft=100,Occupancy=4},
                new VillaDTO { Id = 2, Name = "Beach View",Sqft=300,Occupancy=3 },
            };

    }
}
