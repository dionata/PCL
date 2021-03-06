//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TECPOST_BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientModel()
        {
            this.MeshModel = new HashSet<MeshModel>();
            this.CADModel = new HashSet<CADModel>();
            this.CAMModel = new HashSet<CAMModel>();
        }
    
        public int Id { get; set; }
        public int Codigo { get; set; }
        public long CPF { get; set; }
        public long RG { get; set; }
        public System.DateTime DataNascimento { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public long CEP { get; set; }
        public long Telefone { get; set; }
        public long Celular1 { get; set; }
        public long Celular2 { get; set; }
        public string Email { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeshModel> MeshModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CADModel> CADModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMModel> CAMModel { get; set; }
    }
}
