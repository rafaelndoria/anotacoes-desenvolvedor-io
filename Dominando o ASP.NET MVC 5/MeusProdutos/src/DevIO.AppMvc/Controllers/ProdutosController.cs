using DevIO.AppMvc.Extensions;
using DevIO.AppMvc.Mappings;
using DevIO.AppMvc.ViewModels;
using DevIO.Business.Core.Notifications;
using DevIO.Business.Models.Fornecedores;
using DevIO.Business.Models.Produtos;
using DevIO.Business.Models.Produtos.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DevIO.AppMvc.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapperService _mapper;
        public string ImgPrefixo
        {
            get
            {
                return Guid.NewGuid() + "_";
            }
        }

        public ProdutosController(IProdutoRepository produtoRepository, IProdutoService produtoService, IFornecedorRepository fornecedorRepository, IMapperService mapper, INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var produtosViewModel = await ObterProdutos();

            return View(produtosViewModel);
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return HttpNotFound();


            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var imgPrefixo = ImgPrefixo;
            if (!UploadImagem(produtoViewModel.ImagemUpload, imgPrefixo))
                return View(produtoViewModel);

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            await _produtoService.Adicionar(_mapper.ToEntity(produtoViewModel));

            if (!OperacaoValida())
                View(produtoViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return HttpNotFound();

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var produtoAtualizado = await ObterProduto(produtoViewModel.Id);
            produtoViewModel.Imagem = produtoAtualizado.Imagem;

            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = ImgPrefixo;
                if (!UploadImagem(produtoViewModel.ImagemUpload, imgPrefixo))
                    return View(produtoViewModel);

                produtoAtualizado.Imagem = imgPrefixo + produtoAtualizado.ImagemUpload.FileName;
            }

            produtoAtualizado.Nome = produtoViewModel.Nome;
            produtoAtualizado.Descricao = produtoViewModel.Descricao;
            produtoAtualizado.Valor = produtoViewModel.Valor;
            produtoAtualizado.Ativo = produtoViewModel.Ativo;
            produtoAtualizado.FornecedorId = produtoViewModel.FornecedorId;
            produtoAtualizado.Fornecedor = produtoViewModel.Fornecedor;

            await _produtoService.Atualizar(_mapper.ToEntity(produtoViewModel));

            if (!OperacaoValida())
                return View(produtoAtualizado);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produto", "Deletar")]
        [Route("excluir-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return HttpNotFound();

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Deletar")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return HttpNotFound();

            await _produtoService.Remover(id);

            if (!OperacaoValida())
                return View(produtoViewModel);

            return RedirectToAction("Index");
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.ToViewModel<Produto, ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
            var produtoFornecedores = await PopularFornecedores(produto);

            return produtoFornecedores;
        }

        private async Task<IEnumerable<ProdutoViewModel>> ObterProdutos()
        {
            var produtos = await _produtoRepository.ObterProdutosFornecedores();

            return produtos.Select(p => _mapper.ToViewModel<Produto, ProdutoViewModel>(p));
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            produto.Fornecedores = fornecedores.Select(f => _mapper.ToViewModel<Fornecedor, FornecedorViewModel>(f));

            return produto;
        }

        private bool UploadImagem(HttpPostedFileBase img, string imgPrefixo)
        {
            if (img == null || img.ContentLength <= 0)
            {
                ModelState.AddModelError(string.Empty, "Imagem em formato inaválido!");
                return false;
            }

            var path = Path.Combine(HttpContext.Server.MapPath("~/Imagens"), imgPrefixo + img.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            img.SaveAs(path);

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _produtoRepository.Dispose();
                _produtoService.Dispose();
                _fornecedorRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}