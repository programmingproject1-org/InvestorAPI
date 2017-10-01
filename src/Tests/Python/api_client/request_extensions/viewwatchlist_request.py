#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
import time

from api_client.response_wrappers.viewwatchlist_response_wrapper import ViewWatchlistResponseWrapper

class ViewWatchlistRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/watchlists/WATCHLIST_ID"

	def __init__(self, session, token, watchlist_id):
		self.token = token
		self.watchlist_id = watchlist_id
		self.nonce = int(time.time())
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('WATCHLIST_ID', str(self.watchlist_id))
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('GET', url = self.URL, headers = headers)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = ViewWatchlistResponseWrapper(response)
		response.close()
		return wrapped_response