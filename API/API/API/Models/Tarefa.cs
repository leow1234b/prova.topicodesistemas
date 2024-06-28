/PUT: http://localhost:5225/api/Tarefa/alterar/id_do_Tarefa
app.MapPut("/api/Tarefa/alterar/{id}",
    ([FromRoute] string id,
    [FromBody] Produto produtoAlterado,
    [FromServices] AppDataContext ctx) =>
{
    Produto? produto = ctx.Produtos.Find(id);
    if (produto is null)
    {
        return Results.
            NotFound("Tarefa não encontrada!");
    }
    produto.Nome = TarefaAlterada.Nome;
    produto.Descricao = TarefaAlterada.Descricao;
    produto.Quantidade = produtoAlterado.Quantidade;
    produto.Valor = produtoAlterado.Valor;

    ctx.Produtos.Update(produto);
    ctx.SaveChanges();
    return Results.
        Ok("Tarefa alterado com sucesso!");
});



//GET: http://localhost:5225/api/Tarefa/buscar/id_das_Tarefa
app.MapGet("/api/Tarefa/buscar/{id}", ([FromRoute] string id,
    [FromServices] AppDataContext ctx) =>
{
    //Expressão lambda em c#
    Tarefas? Tarefas =
        ctx.Produtos.FirstOrDefault(x => x.Id == id);
    if (Tarefas is null)
    {
        return Results.NotFound("Tarefa não encontrada!");
    }
    return Results.Ok(Tarefa);
});


//POST: http://localhost:5225/api/tarefa/cadastrar
app.MapPost("/api/Tarefa/cadastrar",
    ([FromBody] Produto produto,
    [FromServices] AppDataContext ctx) =>
{
    //Validação dos atributos do produto
    List<ValidationResult> erros = new
        List<ValidationResult>();
    if (!Validator.TryValidateObject(
        produto, new ValidationContext(Tarefa),
        erros, true))
    {
        return Results.BadRequest(erros);
    }

    
    Tarefa? TarefaBuscado = ctx.Tarefa.
        FirstOrDefault(x => x.Nome == Tarefa.Nome);
    if (produtoBuscado is not null)
    {
        return Results.
            BadRequest("Já existe um Tarefa com o mesmo nome!");
    }



/POST: http://localhost:5225/api/Tarefa/cadastrar
app.MapPost("/api/produto/cadastrar",
    ([FromBody] Produto produto,
    [FromServices] AppDataContext ctx) =>
{
    //Validação dos atributos do produto
    List<ValidationResult> erros = new
        List<ValidationResult>();
    if (!Validator.TryValidateObject(
        produto, new ValidationContext(Tarefa),
        erros, true))
    {
        return Results.BadRequest(erros);
    }

   
    Tarefa? Tarefa = ctx.Tarefas.
        FirstOrDefault(x => x.Nome == produto.Nome);
    if (produtoBuscado is not null)
    {
        return Results.
            BadRequest("Já existe um produto com o mesmo nome!");
    }

    
    ctx.SaveChanges();
    return Results.Created("", produto);
});







