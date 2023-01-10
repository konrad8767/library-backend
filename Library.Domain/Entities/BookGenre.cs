using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Entities
{
    [Flags]
    public enum BookGenre
    {
        Action = 1,
        Comedy = 2,
        Drama = 4, 
        Musical = 8,
        Thriller = 16,
        Horror = 32,
        Romance = 64,
        Fantasy = 128,
        TravelLiterature = 256
    }
}
