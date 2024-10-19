﻿using AutoMapper;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Write
        CreateMap<CommentCreation, Comment>()
            .ConstructUsing(src => new Comment(src.Message, src.CardId));
    }
}
