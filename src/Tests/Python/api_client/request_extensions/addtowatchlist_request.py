#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
import time

from api_client.response_wrappers.addtowatchlist_response_wrapper import AddToWatchlistResponseWrapper

class AddToWatchlistRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/watchlists/WATCHLIST_ID/shares"

	def __init__(self, session, token, watchlist_id, symbol):
		self.token = token
		self.watchlist_id = watchlist_id
		self.symbol = symbol
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('WATCHLIST_ID', str(self.watchlist_id))
		symbol_to_add = {"symbol": self.symbol}
		payload = json.dumps(symbol_to_add, ensure_ascii = False).encode('utf8')
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('POST', url = self.URL, headers = headers, data = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = AddToWatchlistResponseWrapper(response)
		response.close()
		return wrapped_response