//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Es.Udc.DotNet.PracticaMaD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Follow
    {
        public Follow()
        {

        }
        public Follow(long usrId1, long usrId2)
        {
            this.usrId2 = usrId2;
            this.usrId1 = usrId1;
        }

        public long followId { get; set; }
        public long usrId1 { get; set; }
        public long usrId2 { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile UserProfile1 { get; set; }
    }
}
