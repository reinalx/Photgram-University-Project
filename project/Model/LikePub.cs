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
    
    public partial class LikePub
    {
        public long likeId { get; set; }
        public long usrId { get; set; }
        public long postId { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
