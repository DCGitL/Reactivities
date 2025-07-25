type Activity = {
	id: string;
	title: string;
	date: Date;
	description: string;
	category: string;
	isCancelled: boolean;
	city: string;
	venue: string;
	latitude: number;
	longitude: number;
	attendees: Profile[];
	isGoing: boolean;
	isHost: boolean;
	hostId: string;
	hostDisplayName: string;
	hostImageUrl: string;
};

type Profile = {
	id: string;
	displayName: string;
	bio?: string;
	imageUrl?: string;
};
type User = {
	id: string;
	email: string;
	displayName: string;
	imageUrl?: string;
};

type Photo = {
	id: string;
	url: string;
};
type ChatComment = {
	id: string;
	createdAt: Date;
	body: string;
	userId: string;
	displayName: string;
	imageUrl?: string;
};

interface LocationIQSuggestion {
	place_id: string;
	osm_id: string;
	osm_type: string;
	licence: string;
	lat: string;
	lon: string;
	boundingbox: string[];
	class: string;
	type: string;
	display_name: string;
	display_place: string;
	display_address: string;
	address: LocationIQAddress;
}

interface LocationIQAddress {
	name: string;
	city?: string;
	county: string;
	state: string;
	country: string;
	country_code: string;
	road?: string;
	town?: string;
	village?: string;
	neighbourhood?: string;
	suburb?: string;
	postcode?: string;
	house_number?: string;
}
