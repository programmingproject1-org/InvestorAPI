#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json

from api_client.response_wrappers.leaderboard_response_wrapper import LeaderboardResponseWrapper

class LeaderboardRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/leaderboard"

	def __init__(self, session, token, page_number = None, page_size = None):
		self.token = token
		self.page_number = page_number
		self.page_size = page_size
		self.make_request()
		self.session = session



	def make_request(self):
		payload_data = {
			"pageNumber": self.page_number, 
			"pageSize": self.page_size
		}

		payload = json.dumps(payload_data, ensure_ascii = False).encode('utf8')
		
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('GET', url = self.URL, headers = headers, params = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = LeaderboardResponseWrapper(response)
		response.close()
		return wrapped_response