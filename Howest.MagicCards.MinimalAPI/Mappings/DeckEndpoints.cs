using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class DeckEndpoints
    {
        public static void MapDeckEndpoints(this WebApplication deck, string prefix, IMapper mapper, IConfiguration config)
        {
            deck.MapGet($"{prefix}/decks", (IDeckRepository repository) =>
            GetAllDecks(repository, mapper))
                .WithTags("Deck actions")
                .Produces<List<Deck>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            deck.MapGet($"{prefix}/decks/{{id}}", (IDeckRepository repository, long id) =>
            GetDeckById(repository, mapper, id))
                .WithTags("Deck actions")
                .Produces<Deck>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            deck.MapPost($"{prefix}/decks", (IDeckRepository repository, string deckName) =>
            CreateDeck(repository, mapper, deckName))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            deck.MapPut($"{prefix}/decks/{{deckId}}/cards/{{cardId}}", (IDeckRepository repository, int deckId, int cardId) =>
            AddCardToDeck(repository, mapper, deckId, cardId))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deck.MapDelete($"{prefix}/decks/{{deckId}}/cards/{{cardId}}", (IDeckRepository repository, int deckId, int cardId) =>
            RemoveCardFromDeck(repository, mapper, deckId, cardId))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deck.MapDelete($"{prefix}/decks/{{deckId}}", (IDeckRepository repository, int deckId) =>
            ClearDeck(repository, mapper, deckId))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static IResult GetAllDecks(IDeckRepository repository, IMapper mapper)
        {
            try
            {
                IEnumerable<Deck> decks = repository.GetAll();
                List<DeckDTO> deckDTOs = mapper.Map<IEnumerable<Deck>, List<DeckDTO>>(decks);
                return Results.Ok(deckDTOs);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }

        private static IResult GetDeckById(IDeckRepository repository, IMapper mapper, long id)
        {
            try
            {
                Deck deck = repository.GetById(id);
                DeckDTO deckDTO = mapper.Map<Deck, DeckDTO>(deck);
                return Results.Ok(deckDTO);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }

        private static IResult CreateDeck(IDeckRepository repository, IMapper mapper, string deckName)
        {
            try
            {
                // Get the next available ID from the repository
                int nextId = repository.GetNextAvailableId();

                // Create the deck with the next ID
                Deck deck = new Deck { Id = nextId, Name = deckName };

                // Save the deck to the repository
                repository.CreateDeck(deck);

                // Return the created deck with the assigned ID
                return Results.Created($"/decks/{deck.Id}", deck);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }

        private static IResult AddCardToDeck(IDeckRepository repository, IMapper mapper, int deckId, int cardId)
        {
            try
            {
                CardDeck cardDeck = repository.AddCardToDeck(60, deckId, cardId);
                return Results.Ok(cardDeck);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }

        private static IResult RemoveCardFromDeck(IDeckRepository repository, IMapper mapper, int deckId, int cardId)
        {
            try
            {
                CardDeck cardDeck = repository.RemoveCardFromDeck(deckId, cardId);
                return Results.Ok(cardDeck);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }

        private static IResult ClearDeck (IDeckRepository repository, IMapper mapper, int deckId)
        {
            try
            {
                repository.ClearDeck(deckId);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }
    }
}
