﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/autores")] // api/autores => ruta
public class AutoresController: ControllerBase
{
    private readonly ApplicationDbContext context;

    public AutoresController(ApplicationDbContext context)
    {
        this.context = context;
    }
    [HttpGet] // api/autores
    [HttpGet("listado")] // api/autores/listado
    [HttpGet("/listado")] //listado
    public async Task<ActionResult<List<Autor>>> Get()
    {
        return await context.Autores.Include(x => x.Libros).ToListAsync();
    }

    [HttpGet("primero")] // api/autores/primero?nombre=franco&apellido=maurizzio
    public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)
    {
        return await context.Autores.FirstOrDefaultAsync();
    }

    [HttpGet("{id:int}/{param2?}")]
    public async Task<ActionResult<Autor>> Get(int id, string param2)
    {
        var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
        if (autor == null)
        {
            return NotFound();
        }
        return autor;
    }

    [HttpGet("{nombre}")]
    public async Task<ActionResult<Autor>> Get([FromRoute] string nombre)
    {
        var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

        if (autor == null)
        {
            return NotFound();
        }
        return autor;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Autor autor)
    {
        context.Add(autor);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")] //api/autores/{1,2,3,...}
    public async Task<ActionResult> Put(Autor autor, int id)
    {
        if (autor.Id != id)
        {
            return BadRequest("El id del autor no coincide con el id de la URL");
        }

        var existe = await context.Autores.AnyAsync(x => x.Id == id);

        if (!existe)
        {
            return NotFound();
        }

        context.Update(autor);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")] //api/autores/{1,2,3,...}
    public async Task<ActionResult> Delete(int id)
    {
        var existe = await context.Autores.AnyAsync(x => x.Id == id);

        if (!existe)
        {
            return NotFound();
        }

        context.Remove(new Autor() { Id = id });
        await context.SaveChangesAsync();
        return Ok();
    }


}

