using System;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Microsoft.AspNetCore.Mvc;
using WebScrp.DTOs;
using System.Security.Cryptography;
using WebScrp.Repositores;
using WebScrp.Data;
using Microsoft.EntityFrameworkCore;

namespace WebScrp.Controllers;

[ApiController]
[Route("[controller]")]
public class PageHandlerController : ControllerBase
{
    private readonly IParserRepo _parserService;
    private readonly IDecryptorRepo _decryptorService;
    private readonly AppDbContext _context;
    public PageHandlerController(IParserRepo parserService, IDecryptorRepo decryptorRepo,  AppDbContext context) 
    {
        _parserService = parserService;
        _decryptorService = decryptorRepo;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> ScrapePage(InputDataDto inputData) 
    {

        var dto = await _parserService.Parse(inputData);
        var decryptedText = await _decryptorService.Decrypt(inputData);

        if (dto is null || decryptedText is null)
        {
            return BadRequest();
        }


        return Ok(new
        {
            Url = dto.FromUrl,
            Page = dto.Page,
            ElementsCount = dto.elemCount,
            Values = dto.HtmlValues,
            Emails = dto.Emails,
            DecryptedMessage = decryptedText,
        });
    }

    [HttpGet]
    public async Task<IActionResult> ReturnData()
    {
        var data = await _context.Elements.ToListAsync();

        return Ok(data);
    }
 }
