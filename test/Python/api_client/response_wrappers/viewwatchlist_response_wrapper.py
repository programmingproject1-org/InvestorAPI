#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json
from pprint import pprint

class ViewWatchlistResponseWrapper():

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

	def get_id(self):
		return self.get_json_body()["id"]

	def get_name(self):
		return self.get_json_body()["name"]

	def get_all_shares(self):
		return self.get_json_body()["shares"]

	def get_share_by_symbol(self, symbol):
		for share in self.get_all_shares():
			if share["symbol"] == symbol:
				return share
		return None
