<script lang="ts">
  import { onMount } from 'svelte'

  type Plant = {
    id: number
    name: string
    wateringFrequencyDays: number
    species: string
  }

  type PlantDetails = {
    id: number
    name: string
    wateringFrequencyDays: number
    speciesId: number
  }

  type Species = {
    id: number
    name: string
  }

  let plants = $state<Plant[]>([])
  let species = $state<Species[]>([])
  let name = $state('')
  let wateringFrequencyDays = $state(7)
  let speciesId = $state(0)
  let editingId = $state<number | null>(null)
  let error = $state('')
  let isLoading = $state(true)
  let isSaving = $state(false)

  function messageFrom(error: unknown) {
    return error instanceof Error ? error.message : 'Wystąpił nieznany błąd.'
  }

  async function checkResponse(response: Response) {
    if (!response.ok) {
      throw new Error(`Błąd API: ${response.status} ${response.statusText}`)
    }
  }

  async function loadData() {
    error = ''
    isLoading = true

    try {
      const [plantsResponse, speciesResponse] = await Promise.all([
        fetch('/plants/'),
        fetch('/species/'),
      ])

      await checkResponse(plantsResponse)
      await checkResponse(speciesResponse)

      plants = (await plantsResponse.json()) as Plant[]
      species = (await speciesResponse.json()) as Species[]

      if (!species.some((item) => item.id === speciesId)) {
        speciesId = species[0]?.id ?? 0
      }
    } catch (caught) {
      error = messageFrom(caught)
    } finally {
      isLoading = false
    }
  }

  function resetForm() {
    editingId = null
    name = ''
    wateringFrequencyDays = 7
    speciesId = species[0]?.id ?? 0
  }

  async function savePlant(event: SubmitEvent) {
    event.preventDefault()
    error = ''
    isSaving = true

    const payload = {
      name,
      wateringFrequencyDays,
      speciesId,
    }

    try {
      const response = await fetch(
        editingId === null ? '/plants/' : `/plants/${editingId}`,
        {
          method: editingId === null ? 'POST' : 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload),
        },
      )

      await checkResponse(response)
      resetForm()
      await loadData()
    } catch (caught) {
      error = messageFrom(caught)
    } finally {
      isSaving = false
    }
  }

  async function editPlant(plant: Plant) {
    error = ''

    try {
      const response = await fetch(`/plants/${plant.id}`)
      await checkResponse(response)

      const details = (await response.json()) as PlantDetails
      editingId = details.id
      name = details.name
      wateringFrequencyDays = details.wateringFrequencyDays
      speciesId = details.speciesId
    } catch (caught) {
      error = messageFrom(caught)
    }
  }

  async function deletePlant(plant: Plant) {
    if (!window.confirm(`Usunąć roślinę „${plant.name}”?`)) return

    error = ''

    try {
      const response = await fetch(`/plants/${plant.id}`, { method: 'DELETE' })
      await checkResponse(response)
      await loadData()

      if (editingId === plant.id) resetForm()
    } catch (caught) {
      error = messageFrom(caught)
    }
  }

  onMount(() => {
    void loadData()
  })
</script>

<svelte:head>
  <title>Mój kalendarz roślin</title>
  <meta name="description" content="Lista roślin i częstotliwość ich podlewania" />
</svelte:head>

<main class="page">
  <header>
    <p class="eyebrow">DOMOWA ZIELEŃ</p>
    <h1>Mój ogród</h1>
    <p class="subtitle">Rośliny pod dobrą opieką, podlewanie pod kontrolą.</p>
  </header>

  <section class="panel">
    <h2>{editingId === null ? 'Dodaj roślinę' : 'Edytuj roślinę'}</h2>

    <form onsubmit={savePlant}>
      <label>
        Nazwa rośliny
        <input bind:value={name} maxlength="50" required placeholder="np. Monstera" />
      </label>

      <label>
        Gatunek
        <select bind:value={speciesId} required disabled={species.length === 0}>
          {#each species as item (item.id)}
            <option value={item.id}>{item.name}</option>
          {/each}
        </select>
      </label>

      <label>
        Podlewanie co (dni)
        <input
          type="number"
          bind:value={wateringFrequencyDays}
          min="1"
          required
        />
      </label>

      <div class="form-actions">
        <button type="submit" disabled={isSaving || species.length === 0}>
          {isSaving ? 'Zapisywanie…' : editingId === null ? 'Dodaj roślinę' : 'Zapisz zmiany'}
        </button>

        {#if editingId !== null}
          <button class="secondary" type="button" onclick={resetForm}>Anuluj</button>
        {/if}
      </div>
    </form>

    {#if species.length === 0 && !isLoading}
      <p class="hint">API nie zwróciło gatunków — bez nich nie można dodać rośliny.</p>
    {/if}
  </section>

  <section class="plants">
    <div class="section-heading">
      <h2>Moje rośliny</h2>
      <span>{plants.length}</span>
    </div>

    {#if error}
      <p class="error" role="alert">{error}</p>
    {/if}

    {#if isLoading}
      <p class="empty">Ładowanie roślin…</p>
    {:else if plants.length === 0}
      <p class="empty">Lista jest pusta. Dodaj swoją pierwszą roślinę.</p>
    {:else}
      <ul>
        {#each plants as plant (plant.id)}
          <li class="plant-card">
            <div class="plant-icon" aria-hidden="true">🌱</div>
            <div class="plant-info">
              <h3>{plant.name}</h3>
              <p>{plant.species}</p>
              <p class="frequency">Podlewanie co {plant.wateringFrequencyDays} dni</p>
            </div>
            <div class="card-actions">
              <button class="secondary" onclick={() => editPlant(plant)}>Edytuj</button>
              <button class="danger" onclick={() => deletePlant(plant)}>Usuń</button>
            </div>
          </li>
        {/each}
      </ul>
    {/if}
  </section>
</main>