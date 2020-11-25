﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticalStore articalStore;

        public ArticleController(IArticalStore articalStore)
        {
            this.articalStore = articalStore;
        }

        [HttpGet]
        public List<Article> List()
        {
            return ArticleStoreWillReplaceInFuture.Articles.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Article>> Create(Article article)
        {
            if (article.UserName != null)
            {
                if (!UserStoreWillReplaceInFuture.Users.Exists(_ => article.UserName == _.Name))
                {
                    UserStoreWillReplaceInFuture.Users.Add(new User(article.UserName));
                }

                articalStore.Articles.Add(article);
            }

            //IArticalStore articalStore = new ArticalStore();
            //articalStore.Articles.Add(article);

            return CreatedAtAction(nameof(GetById), new { id= article.Id }, article);
        }

        [HttpGet("{id}")]
        public Article GetById(Guid id)
        {
            var foundArticle = articalStore.Articles.FirstOrDefault(article => article.Id == id);
            return foundArticle;
        }
    }
}