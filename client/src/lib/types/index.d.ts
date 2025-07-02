 type Activity = {
    id: string
    title: string
    date: Date
    description: string
    category: string
    isCancelled: boolean
    city: string
    venue: string
    latitude: number
    longitude: number
  }

  export interface LocationIQSuggestion  {
    place_id: string
    osm_id: string
    osm_type: string
    licence: string
    lat: string
    lon: string
    boundingbox: string[]
    class: string
    type: string
    display_name: string
    display_place: string
    display_address: string
    address: LocationIQAddress
  }
  
  export interface  LocationIQAddress {
    name: string
    city?: string
    county: string
    state: string
    country: string
    country_code: string
    road?: string
    town?: string
    village?:string
    neighbourhood?: string
    suburb?: string
    postcode?: string
    house_number?: string
  }
  