using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class SeedData
    {
        public static void Initialize(SubastaContext context)
        {
            UsuarioSeed.Seed(context);
            BilleteraSeed.Seed(context);
            CategoriaSeed.Seed(context);
            SubastaSeed.Seed(context);
            PujaSeed.Seed(context);
            TransaccionLedgerSeed.Seed(context);
        }
    }
}
