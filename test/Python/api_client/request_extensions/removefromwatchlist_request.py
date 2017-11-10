#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
import time

from api_client.response_wrappers.removefromwatchlist_response_wrapper import RemoveFromWatchlistResponseWrapper

class RemoveFromWatchlistRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/watchlists/WATCHLIST_ID/shares/SYMBOL"

	def __init__(self, session, token, watchlist_id, symbol):
		self.token = token
		self.watchlist_id = watchlist_id
		self.symbol = symbol
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('WATCHLIST_ID', str(self.watchlist_id))
		self.URL = self.URL.replace('SYMBOL', str(self.symbol))
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('DELETE', url = self.URL, headers = headers)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = RemoveFromWatchlistResponseWrapper(response)
		response.close()
		return wrapped_response