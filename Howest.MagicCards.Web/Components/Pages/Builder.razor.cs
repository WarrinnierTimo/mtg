using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages;

public partial class Builder
{
    private string _message = string.Empty;
    private string _currentUrl;

    private IEnumerable<CardReadDTO> _cards = null;
    private IEnumerable<RarityReadDTO> _rarities = null;
    private IEnumerable<SetReadDTO> _sets = null;
    private IEnumerable<ArtistReadDTO> _artists = null;

    private IEnumerable<DeckDTO> _decks = null;
    private DeckDTO _selectedDeck = null;
    private IEnumerable<CardDetailReadDTO> _deckCards = null;
    private CardDetailReadDTO _selectedCard = null;

    private Boolean _sortingAsc = true;

    private int _pageNumber = 1;
    private int _totalPages = 1;

    private Boolean isAddDisabled = false;

    private readonly JsonSerializerOptions _jsonOptions;
    private HttpClient _httpClient;
    private HttpClient _httpClientDecks;

    #region Services
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IMapper Mapper { get; set; }

    [Inject]
    public ProtectedSessionStorage Session { get; set; }
    #endregion

    #region Parameters
    [Parameter]
    public string Name { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string Type { get; set; }

    [Parameter]
    public string SetCode { get; set; }

    [Parameter]
    public string RarityCode { get; set; }

    [Parameter]
    public int? ArtistId { get; set; }

    [Parameter]
    public string SortOrder { get; set; } = "asc";

    [Parameter]
    public int DeckId { get; set; }

    [Parameter]
    public string DeckName { get; set; }
    #endregion

    public Builder()
    {
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    protected override async Task OnInitializedAsync()
    {
        _currentUrl = NavigationManager.Uri;
        _httpClient = HttpClientFactory.CreateClient("CardsAPI");
        _httpClientDecks = HttpClientFactory.CreateClient("DecksAPI");

        await SetRarities();
        await SetSets();
        await SetArtists();
        await ShowAllCards();
        await SetDecks();
        await ShowDeckCards();
    }

    private async Task ChangePage(int newPage)
    {
        _pageNumber = newPage; // Update the current page

        // Fetch the data for the new page
        await ShowAllCards();
    }

    private async Task ShowAllCards()
    {
        string apiUrl = $"cards";
        if (!string.IsNullOrEmpty(Name))
        {
            apiUrl += $"?Name={Name}";
        }
        if (!string.IsNullOrEmpty(Text))
        {
            apiUrl += string.IsNullOrEmpty(Name) ? $"?Text={Text}" : $"&Text={Text}";
        }
        if (!string.IsNullOrEmpty(Type))
        {
            apiUrl += string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Text) ? $"?Type={Type}" : $"&Type={Type}";
        }
        if (!string.IsNullOrEmpty(SetCode) && SetCode != "all")
        {
            apiUrl += string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(Type) ? $"?SetCode={SetCode}" : $"&SetCode={SetCode}";
        }
        if (!string.IsNullOrEmpty(RarityCode) && RarityCode != "all")
        {
            apiUrl += string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(SetCode) ? $"?RarityCode={RarityCode}" : $"&RarityCode={RarityCode}";
        }
        if (ArtistId.HasValue)
        {
            apiUrl += string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(SetCode) && string.IsNullOrEmpty(RarityCode) ? $"?ArtistId={ArtistId}" : $"&ArtistId={ArtistId}";
        }
        if (!string.IsNullOrEmpty(SortOrder))
        {
            apiUrl += string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(SetCode) && string.IsNullOrEmpty(RarityCode) && !ArtistId.HasValue ? $"?SortOrder={SortOrder}" : $"&SortOrder={SortOrder}";
        }
        apiUrl += apiUrl.Contains("?") ? $"&PageNumber={_pageNumber}" : $"?PageNumber={_pageNumber}";
        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
        string apiResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            PagedResponse<IEnumerable<CardReadDTO>> cards = 
                JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardReadDTO>>>(apiResponse, _jsonOptions);
            _cards = cards?.Data;
            _totalPages = cards.TotalPages;
        }
        else
        {
            _cards = new List<CardReadDTO>();
            _message = $"Error: {response.ReasonPhrase}";
        }
    }

    private async Task SetRarities()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("rarities");
        string apiResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            IEnumerable<RarityReadDTO> rarities = JsonSerializer.Deserialize<IEnumerable<RarityReadDTO>>(apiResponse, _jsonOptions);
            _rarities = rarities;
        }
        else
        {
            _rarities = new List<RarityReadDTO>();
            _message = $"Error: {response.ReasonPhrase}";
        }
    }

    private async Task SetSets()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("sets");
        string apiResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            IEnumerable<SetReadDTO> sets = JsonSerializer.Deserialize<IEnumerable<SetReadDTO>>(apiResponse, _jsonOptions);
            _sets = sets;
        }
        else
        {
            _sets = new List<SetReadDTO>();
            _message = $"Error: {response.ReasonPhrase}";
        }
    }

    private async Task SetArtists()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("artists");
        string apiResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            IEnumerable<ArtistReadDTO> artists = JsonSerializer.Deserialize<IEnumerable<ArtistReadDTO>>(apiResponse, _jsonOptions);
            _artists = artists;
        }
        else
        {
            _artists = new List<ArtistReadDTO>();
            _message = $"Error: {response.ReasonPhrase}";
        }
    }

    private async Task SortCards()
    {
        SortOrder = _sortingAsc ? "asc" : "desc";
        _sortingAsc = !_sortingAsc;

        await ShowAllCards();
    }

    private async Task SetDecks()
    {
        HttpResponseMessage response = await _httpClientDecks.GetAsync("decks");
        string apiResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            IEnumerable<DeckDTO> decks = JsonSerializer.Deserialize<IEnumerable<DeckDTO>>(apiResponse, _jsonOptions);
            _decks = decks;
        }
        else
        {
            _decks = new List<DeckDTO>();
            _message = $"Error: {response.ReasonPhrase}";
        }
    }

    private async Task ShowDeckCards()
    {
        if (DeckId != 0)
        {
            HttpResponseMessage response = await _httpClientDecks.GetAsync($"decks/{DeckId}");
            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                DeckDTO selectedDeck = JsonSerializer.Deserialize<DeckDTO>(apiResponse, _jsonOptions);
                _selectedDeck = selectedDeck;
                IEnumerable<CardDeckDTO> deckCards = selectedDeck.CardDecks;

                Boolean isFull = isDeckFull(deckCards);
                isAddDisabled = isFull;

                List<CardDetailReadDTO> cards = new List<CardDetailReadDTO>();
                _deckCards = new List<CardDetailReadDTO>();
                foreach (CardDeckDTO cardDeck in deckCards)
                {
                    CardDetailReadDTO card = await getCardById(cardDeck.Id, true);  
                    cards.Add(card);
                }
                _deckCards = cards;
            }
            else
            {
                _selectedDeck = new DeckDTO();
                _message = $"Error: {response.ReasonPhrase}";
            }
        } else
        {
            _selectedDeck = null;
        }
    }

    private Boolean isDeckFull(IEnumerable<CardDeckDTO> deckCards)
    {
        int totalAmount = 0;
        foreach (CardDeckDTO cardDeck in deckCards)
        {
            totalAmount += cardDeck.Amount;
        }
        return totalAmount >= 60;
    }

    private async Task<CardDetailReadDTO> getCardById(long cardId, Boolean isDeck = false)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"cards/{cardId}");
        string apiResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            Response<CardDetailReadDTO> card = JsonSerializer.Deserialize<Response<CardDetailReadDTO>>(apiResponse, _jsonOptions);
            if (isDeck)
            {
                return card.Data;
            } else
            {
                _selectedCard = card.Data;
                StateHasChanged();
                return card.Data;
            }
        }
        else
        {
            return null;
        }
    }

    private async Task AddCardToDeck(long cardId)
    {
        if (DeckId != 0)
        {
            CardDeckDTO cardDeck = new CardDeckDTO();
            HttpResponseMessage response = await _httpClientDecks.PutAsJsonAsync($"decks/{DeckId}/cards/{cardId}", cardDeck);
            if (response.IsSuccessStatusCode)
            {
                await ShowDeckCards();
                StateHasChanged();
                isAddDisabled = true;
            }
            else
            {
                isAddDisabled = false;
                _message = $"Error: {response.ReasonPhrase}";
            }
        }
    }

    private async Task RemoveCardFromDeck(int cardId)
    {
        if (DeckId != 0)
        {
            HttpResponseMessage response = await _httpClientDecks.DeleteAsync($"decks/{DeckId}/cards/{cardId}");
            if (response.IsSuccessStatusCode)
            {
                await ShowDeckCards();
                StateHasChanged();
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }
        }
    }

    private async Task AddDeck()
    {
        HttpResponseMessage response = await _httpClientDecks.PostAsJsonAsync($"decks?deckName={DeckName}", DeckName);
        if (response.IsSuccessStatusCode)
        {
            await SetDecks();
            StateHasChanged();
            DeckName = string.Empty;
        }
        else
        {
            _message = $"Error: {response.ReasonPhrase}";
        }
    }

    private async Task ClearDeck()
    {
        HttpResponseMessage response = await _httpClientDecks.DeleteAsync($"decks/{_selectedDeck.Id}");
        if (response.IsSuccessStatusCode)
        {
            await ShowDeckCards();
            StateHasChanged();
        }
        else
        {
            _message = $"Error: {response.ReasonPhrase}";
        }
    }
}
