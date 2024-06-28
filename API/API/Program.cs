using System.ComponentModel.DataAnnotations;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Registrar o serviço de banco de dados na aplicação
builder.Services.AddDbContext<AppDataContext>();

//Configurar a política de CORS
builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

var app = builder.Build();

//EndPoints - Funcionalidades
//GET: http://localhost:5225/
app.MapGet("/", () => "Minha primeira API em C# com watch");

//GET: http://localhost:5225/api/produto/listar
app.MapGet("/api/produto/listar",
    ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Produtos.Any())
    {
        return Results.Ok(ctx.Produtos.Include(x => x.Categoria).ToList());
    }
    return Results.NotFound("Tabela vazia!");
});

//GET: http://localhost:5225/api/produto/buscar/id_do_produto
app.MapGet("/api/produto/buscar/{id}", ([FromRoute] string id,
    [FromServices] AppDataContext ctx) =>
{
    //Expressão lambda em c#
    Produto? produto =
        ctx.Produtos.FirstOrDefault(x => x.Id == id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado!");
    }
    return Results.Ok(produto);
});

//POST: http://localhost:5225/api/produto/cadastrar
app.MapPost("/api/produto/cadastrar",
    ([FromBody] Produto produto,
    [FromServices] AppDataContext ctx) =>
{
    //Validação dos atributos do produto
    List<ValidationResult> erros = new
        List<ValidationResult>();
    if (!Validator.TryValidateObject(
        produto, new ValidationContext(produto),
        erros, true))
    {
        return Results.BadRequest(erros);
    }

    //RN: Não permitir produtos com o mesmo nome
    Produto? produtoBuscado = ctx.Produtos.
        FirstOrDefault(x => x.Nome == produto.Nome);
    if (produtoBuscado is not null)
    {
        return Results.
            BadRequest("Já existe um produto com o mesmo nome!");
    }

    produto.Categoria = ctx.Categorias.
        Find(produto.CategoriaId);
    //Adicionar o produto dentro do banco de dados
    ctx.Produtos.Add(produto);
    ctx.SaveChanges();
    return Results.Created("", produto);
});

//DELETE: http://localhost:5225/api/produto/deletar/id_do_produto
app.MapDelete("/api/produto/deletar/{id}",
    ([FromRoute] string id,
    [FromServices] AppDataContext ctx) =>
{
    Produto? produto = ctx.Produtos.Find(id);
    if (produto is null)
    {
        return Results.
            NotFound("Produto não encontrado!");
    }
    ctx.Produtos.Remove(produto);
    ctx.SaveChanges();
    return Results.
        Ok(ctx.Produtos.ToList());
});

//PUT: http://localhost:5225/api/produto/alterar/id_do_produto
app.MapPut("/api/produto/alterar/{id}",
    ([FromRoute] string id,
    [FromBody] Produto produtoAlterado,
    [FromServices] AppDataContext ctx) =>
{
    Produto? produto = ctx.Produtos.Find(id);
    if (produto is null)
    {
        return Results.
            NotFound("Produto não encontrado!");
    }
    produto.Nome = produtoAlterado.Nome;
    produto.Descricao = produtoAlterado.Descricao;
    produto.Quantidade = produtoAlterado.Quantidade;
    produto.Valor = produtoAlterado.Valor;

    ctx.Produtos.Update(produto);
    ctx.SaveChanges();
    return Results.
        Ok("Produto alterado com sucesso!");
});

//POST: http://localhost:5225/api/categoria/cadastrar
app.MapPost("/api/categoria/cadastrar",
    ([FromBody] Categoria categoria,
    [FromServices] AppDataContext ctx) =>
{

    ctx.Categorias.Add(categoria);
    ctx.SaveChanges();
    return Results.Created("", categoria);
});

//GET: http://localhost:5225/api/categoria/listar
app.MapGet("/api/categoria/listar",
    ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Categorias.Any())
    {
        return Results.Ok(ctx.Categorias.ToList());
    }
    return Results.NotFound("Tabela vazia!");
});

app.UseCors("Acesso Total");
app.Run();