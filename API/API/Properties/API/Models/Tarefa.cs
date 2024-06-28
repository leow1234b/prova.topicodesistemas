namespace API.Models;

public class Tarefa
{
    public string TarefaId { get; set; } = Guid.NewGuid().ToString();
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public Categoria? Categoria { get; set; }
    public string? CategoriaId { get; set; }
    public string? Status { get; set; } = "Não iniciada";

    ### Listar Produtos
GET http://localhost:5225/api/Tarefas/listar
   <Route
             path="/pages/produto/listar"
            element={<TarefaListar />}
          />
POST http://localhost:5225/api/Tarefas/cadastrar
Content-Type: application/json

{
    "nome" : "Arroz A",
    "descricao" : "Alimento",
    "valor" : 15,
    "quantidade" : 1500,
    "categoriaId" : "ecfbe786-b393-4b06-afe4-0c65b2c44a3b"
}
}

   <Route
            path="/pages/Tarefa/alterar/:id"
            element={<TarefaAlterar />}
          />
        </Routes>

