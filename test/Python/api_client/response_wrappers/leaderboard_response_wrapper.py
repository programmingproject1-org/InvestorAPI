#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json

class LeaderboardResponseWrapper():

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

	def get_all_items(self):
		if self.get_json_body is None:
			return None

		return self.get_json_body()["items"]

	def get_page_number(self):
		if self.get_json_body is None:
			return None

		return self.get_json_body()["pageNumber"]

	def get_page_size(self):
		if self.get_json_body is None:
			return None

		return self.get_json_body()["pageSize"] 

	def get_page_count(self):
		if self.get_json_body is None:
			return None

		return self.get_json_body()["totalPageCount"]

	def get_row_count(self):
		if self.get_json_body is None:
			return None

		return self.get_json_body()["totalRowCount"] 