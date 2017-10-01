#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
import time

from api_client.response_wrappers.sellshare_response_wrapper import SellShareResponseWrapper

class SellShareRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/accounts/ACCOUNT_ID/orders"

	def __init__(self, session, token, account_id, symbol, quantity, side = "Sell"):
		self.token = token
		self.account_id = account_id
		self.side = side
		self.symbol = symbol
		self.quantity = quantity
		self.nonce = int(time.time())
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('ACCOUNT_ID', str(self.account_id))
		order = {"side": self.side, "symbol": self.symbol, "quantity": self.quantity, "nonce": self.nonce}
		payload = json.dumps(order, ensure_ascii = False).encode('utf8')
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('POST', url = self.URL, headers = headers, data = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = SellShareResponseWrapper(response)
		response.close()
		return wrapped_response