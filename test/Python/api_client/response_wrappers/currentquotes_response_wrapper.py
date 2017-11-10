#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json

class CurrentQuotesResponseWrapper():

	SUCCESS_STATUS = 200

	def __init__(self, response):
		self.response = response

	def get_http_status(self):
		return self.response.status_code

	def get_json_body(self):
		try:
			body = self.response.json()
		except ValueError:
			body = None
		return body

	def get_all_quotes(self):
		return self.get_json_body()

	def get_quote_by_symbol(self, symbol):
		for quote in self.get_all_quotes():
			if quote["symbol"] == symbol:
				return quote
		return None
