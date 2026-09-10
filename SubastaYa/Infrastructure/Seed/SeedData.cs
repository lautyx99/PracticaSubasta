using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class SeedData
    {
        public static void Initialize(SubastaContext context)
        {

            // 1. Primarias (Sin FKs)
            UsuarioSeed.Seed(context);
            CategoriaSeed.Seed(context);

            // 2. Secundarias (Dependen de Usuarios y Categorías)
            SubastaSeed.Seed(context);

            // 3. Opcional: BilleteraSeed o PujaSeed si aplica
            BilleteraSeed.Seed(context);
            PujaSeed.Seed(context);
            TransaccionLedgerSeed.Seed(context);
        }
    }
}
