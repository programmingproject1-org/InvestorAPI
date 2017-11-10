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