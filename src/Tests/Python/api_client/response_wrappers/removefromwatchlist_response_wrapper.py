#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json
from pprint import pprint

class RemoveFromWatchlistResponseWrapper():

	SUCCESS_STATUS = 204

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
		
	def get_all_shares(self):
		return self.get_json_body()

	def get_share_by_symbol(self, symbol):
		for share in self.get_all_shares():
			if share["symbol"] == symbol:
				return share
		return None