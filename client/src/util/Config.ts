const Config = {
	userBaseURL: import.meta.env.VITE_BASE_URL as string,
	userLocationBaseURL: import.meta.env.VITE_BASE_LOCATION as string,
	userCommentBaseURL: import.meta.env.VITE_COMMENT_URL as string,
	clientID: import.meta.env.VITE_GITHUB_CLIENT_ID as string,
	redirectUrl: import.meta.env.VITE_REDIRECT_URL as string,
	githubOAuthUrl: import.meta.env.VITE_GITHUB_OAUTH_URL as string,
};

export default Config;
