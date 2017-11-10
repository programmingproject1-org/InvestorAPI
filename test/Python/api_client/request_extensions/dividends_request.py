#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
from pprint import pprint

from api_client.response_wrappers.dividends_response_wrapper import DividendsResponseWrapper

class DividendsRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/shares/SYMBOL/dividends"

	def __init__(self, session, token, symbol):

		self.token = token
		self.symbol = symbol
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('SYMBOL', str(self.symbol))
		payload_data = {}
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		#pprint(headers)
		super().__init__('GET', url = self.URL, headers = headers, params = payload_data)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = DividendsResponseWrapper(response)
		response.close()
		return wrapped_response