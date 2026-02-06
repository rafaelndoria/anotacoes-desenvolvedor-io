using DevIO.AppMvc.Mappings;
using DevIO.Business.Models.Fornecedores;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevIO.AppMvc.ViewModels
{
    public class FornecedorViewModel : IMapFrom<Fornecedor>, IMapTo<Fornecedor>
    {
        public FornecedorViewModel()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        public string Documento { get; set; }

        [Display(Name = "Tipo")]
        public TipoFornecedor TipoFornecedor { get; set; }

        public EnderecoViewModel Endereco { get; set; }

        [Display(Name = "Ativo?")]
        public bool Ativo { get; set; }

        public IEnumerable<ProdutoViewModel> Produtos { get; set; }

        public void FromEntity(Fornecedor entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            Nome = entity.Nome;
            Documento = entity.Documento;
            TipoFornecedor = entity.TipoFornecedor;
            Ativo = entity.Ativo;

            if (entity.Endereco == null) return;
            Endereco = new EnderecoViewModel()
            {
                Id = entity.Endereco.Id,
                Logradouro = entity.Endereco.Logradouro,
                Numero = entity.Endereco.Numero,
                Complemento = entity.Endereco.Complemento,
                Cep = entity.Endereco.Cep,
                Bairro = entity.Endereco.Bairro,
                Cidade = entity.Endereco.Cidade,
                Estado = entity.Endereco.Estado,
                FornecedorId = entity.Id
            };
        }

        public Fornecedor ToEntity()
        {
            var fornecedor = new Fornecedor()
            {   
                Id = Id,
                Nome = Nome,
                Documento = Documento,
                TipoFornecedor = TipoFornecedor,
                Ativo = Ativo,
                Endereco = new Endereco()
                {   
                    Id = Endereco.Id,
                    Logradouro = Endereco.Logradouro,
                    Numero = Endereco.Numero,
                    Complemento = Endereco.Complemento,
                    Cep = Endereco.Cep,
                    Bairro = Endereco.Bairro,
                    Cidade = Endereco.Cidade,
                    Estado = Endereco.Estado,
                    FornecedorId = Id
                }
            };

            return fornecedor;
        }
    }
}