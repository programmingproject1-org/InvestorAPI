#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json

from api_client.response_wrappers.currentquotes_response_wrapper import CurrentQuotesResponseWrapper

class CurrentQuotesRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/shares/quotes"
	SYMBOL_SEPARATOR = ','

	def __init__(self, session, token, symbols):
		self.token = token
		self.symbols = self.format_symbols(symbols)
		self.make_request()
		self.session = session

	def format_symbols(self, symbols):
		if isinstance(symbols, list):
			return self.SYMBOL_SEPARATOR.join(symbols)
		return symbols

	def make_request(self):
		payload = {"symbols": self.symbols}
		headers = {
			"Content-Type": "application/json",
			"charset": "UTF-8",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('GET', url = self.URL, headers = headers, params = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = CurrentQuotesResponseWrapper(response)
		response.close()
		return wrapped_response