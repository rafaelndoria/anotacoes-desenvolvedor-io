using DevIO.AppMvc.Extensions;
using DevIO.AppMvc.Mappings;
using DevIO.Business.Models.Produtos;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DevIO.AppMvc.ViewModels
{
    public class ProdutoViewModel : IMapTo<Produto>, IMapFrom<Produto>
    {
        public ProdutoViewModel()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Imagem do Produto")]
        public HttpPostedFileBase ImagemUpload { get; set; }

        public string Imagem { get; set; }

        [Moeda]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }

        public FornecedorViewModel Fornecedor { get; set; }

        public IEnumerable<FornecedorViewModel> Fornecedores { get; set; }

        public void FromEntity(Produto entity)
        {
            Id = entity.Id;
            FornecedorId = entity.FornecedorId;
            Nome = entity.Nome;
            Descricao = entity.Descricao;
            Imagem = entity.Imagem;
            Valor = entity.Valor;
            DataCadastro = entity.DataCadastro;
            Ativo = entity.Ativo;

            if (entity.Fornecedor != null)
            {
                Fornecedor = new FornecedorViewModel();
                Fornecedor.FromEntity(entity.Fornecedor);
            }
        }

        public Produto ToEntity()
        {
            return new Produto
            {
                Nome = Nome,
                Descricao = Descricao,
                Imagem = Imagem,
                Valor = Valor,
                Ativo = Ativo,
                FornecedorId = FornecedorId,
                Fornecedor = Fornecedor != null ? Fornecedor.ToEntity() : null
            };
        }
    }
}